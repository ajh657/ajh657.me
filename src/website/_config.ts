import lume from "lume/mod.ts";
import nav from "lume/plugins/nav.ts";
import { exists } from "jsr:@std/fs@^1.0.24/exists";
import { syncHobbyLayouts } from "./submoduleSync.ts";

const site = lume({ src: "./src", prettyUrls: false });

site.use(nav());

if (await exists("./src/hobbies")) {
	if (await exists("./src/hobbies/hobbies.config.ts")) {
		var configModule = await import("./src/hobbies/hobbies.config.ts");
		configModule.ConfigureHobbies(site);
	}

	site.addEventListener("beforeBuild", syncHobbyLayouts);
	site.addEventListener("beforeUpdate", syncHobbyLayouts);
}

site.ignore("hobbies/.github");
site.ignore("hobbies/README.md");

site.add("_well-known", "_well-known");
site.add("img", "src/img");
site.add("css", "src/css");

export default site;
