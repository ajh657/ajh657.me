async function getStory() {
	const urlParams = new URLSearchParams(window.location.search);
	const story = urlParams.get("story");

	if (story !== null) {
		var storyAddress = "./HTML/" + story + ".html";
        var storyhtml =  await fetch(storyAddress)

        if (storyhtml.ok) {
            var storytext = await storyhtml.text()
            document.getElementById("story-container").innerHTML = storytext;
        
            document.title = document.querySelector("#title-block-header > .title").innerText;
        }
	}
}

window.addEventListener("DOMContentLoaded", () => {
    getStory().then(() => {});
});