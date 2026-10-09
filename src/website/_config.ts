import lume from "lume/mod.ts";
import nav from "lume/plugins/nav.ts";
import { join, relative } from "jsr:@std/path@1";

const site = lume({ src: "./src", prettyUrls: false });

site.use(nav());

const hobbyLayouts = join(Deno.cwd(), "src/hobbies/_layout");
const includedLayouts = join(Deno.cwd(), "src/_includes/hobbies");

async function copyChangedFile(
	source: string,
	destination: string,
): Promise<void> {
	const sourceContents: string = await Deno.readTextFile(source);
	let destinationContents: string | undefined;

	try {
		destinationContents = await Deno.readTextFile(destination);
	} catch (error) {
		if (!(error instanceof Deno.errors.NotFound)) {
			throw error;
		}
	}

	// Avoid rewriting the file and retriggering Lume's watcher.
	if (sourceContents === destinationContents) {
		return;
	}

	await Deno.mkdir(join(destination, ".."), { recursive: true });
	await Deno.writeTextFile(destination, sourceContents);
	console.log(
		`Copied ${relative(Deno.cwd(), source)} → ${relative(Deno.cwd(), destination)}`,
	);
}

async function syncLayoutDirectory(
	sourceDirectory: string,
	destinationDirectory: string,
): Promise<void> {
	await Deno.mkdir(destinationDirectory, { recursive: true });

	const sourceFiles = new Set<string>();

	for await (const entry of Deno.readDir(sourceDirectory)) {
		const sourcePath = join(sourceDirectory, entry.name);
		const destinationPath = join(destinationDirectory, entry.name);

		if (entry.isDirectory) {
			await syncLayoutDirectory(sourcePath, destinationPath);
		} else if (entry.isFile) {
			sourceFiles.add(entry.name);
			await copyChangedFile(sourcePath, destinationPath);
		}
	}

	// Remove copied files that no longer exist in the submodule.
	for await (const entry of Deno.readDir(destinationDirectory)) {
		if (!sourceFiles.has(entry.name)) {
			await Deno.remove(join(destinationDirectory, entry.name), {
				recursive: true,
			});
		}
	}
}

async function syncHobbyLayouts(): Promise<void> {
	try {
		await Deno.stat(hobbyLayouts);
	} catch (error) {
		if (error instanceof Deno.errors.NotFound) {
			return;
		}

		throw error;
	}

	await syncLayoutDirectory(hobbyLayouts, includedLayouts);
}

site.addEventListener("beforeBuild", syncHobbyLayouts);
site.addEventListener("beforeUpdate", syncHobbyLayouts);

site.add("_well-known", "_well-known");
site.add("img", "src/img");
site.add("css", "src/css");

export default site;
