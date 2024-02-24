async function GetStory() {
	const urlParams = new URLSearchParams(window.location.search);
	const story = urlParams.get("story");

	if (story !== null) {
		var storyAddress = "./HTML/" + story + ".html";
		var storyhtml = await fetch(storyAddress);

		if (storyhtml.ok) {
			var storytext = await storyhtml.text();
			document.getElementById("story-container").innerHTML = storytext;

			var title = document.querySelector("#title-block-header > .title").innerText;

            document.title = title;
            updateBreadcrumb(title);

			if (posthog !== undefined) {
				posthog.capture("Story Loaded", { story: story });
			}
		} else {
			Error();
		}
	} else {
		Error();
	}
}

function Error() {
	document.getElementById("story-container").innerHTML = "<h1>404</h1><p>Story not found.</p>";
    updateBreadcrumb("Story not found");
    if (posthog !== undefined) {
        posthog.capture("Story failed to load", { story: story });
    }
}

function updateBreadcrumb(breadcrumbString) {
    var bread = document.getElementById("current-sotry-breadcrumb");
    bread.innerHTML = breadcrumbString;
}

function loadFont() {
    var cookie = getCookie("ffxiv_story_font");

    document.getElementById("font-default").disabled = false;
    document.getElementById("font-OpenDyslexic").disabled = false;

    if (cookie === null || cookie === "default") {
        if (cookie === null) {
            setCookie("ffxiv_story_font", "default");
        }
        document.getElementById("font-default").disabled = true;
        document.getElementById("story-container").className = "story-container story-font";
    }
    
    if (cookie === "OpenDyslexic") {
        document.getElementById("story-container").className = "story-container story-font-OpenDyslexic";
        document.getElementById("font-OpenDyslexic").disabled = true;
    }
}

function updateFont(type) {
    setCookie("ffxiv_story_font", type);
    loadFont();
}

function setCookie(name,value,days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days*24*60*60*1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "")  + expires + "; path=/";
}
function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for(var i=0;i < ca.length;i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') c = c.substring(1,c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
    }
    return null;
}

window.addEventListener("DOMContentLoaded", () => GetStory(), false);
