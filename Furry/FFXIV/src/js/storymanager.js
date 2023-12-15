function getStory() {
    const urlParams = new URLSearchParams(window.location.search);
    const story = urlParams.get('story');

    if (story !== null) {
        var storyAddress = "./HTML/" + story + ".html";
        console.log(storyAddress);
        document.getElementById("story-container").src = storyAddress;
    }
}

window.addEventListener('load', function () {
    getStory();
  })
  