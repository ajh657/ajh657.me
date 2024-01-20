async function GetStory() {
	const urlParams = new URLSearchParams(window.location.search);
	const story = urlParams.get("story");

	if (story !== null) {
		var storyAddress = "./HTML/" + story + ".html";
		var storyhtml = await fetch(storyAddress);

		if (storyhtml.ok) {
			var storytext = await storyhtml.text();
			document.getElementById("story-container").innerHTML = storytext;

			document.title = document.querySelector("#title-block-header > .title").innerText;
		} else {
			document.getElementById("story-container").innerHTML = "<h1>404</h1><p>Story not found.</p>";
		}
	} else {
		document.getElementById("story-container").innerHTML = "<h1>404</h1><p>Story not found.</p>";
	}

    if (posthog !== undefined) {
        posthog.capture("Story Loaded", {"story": story});
    }
}

window.addEventListener("DOMContentLoaded", () => GetStory(), false);
