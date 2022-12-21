async function json2website(url) {
    var absoluteURL = new URL(url, document.baseURI).href

    var responce = await fetch(absoluteURL)
    var json = await responce.text()

    var parsedData = JSON.parse(json)

    Object.keys(parsedData).forEach(function (key) {
        if (key == "links") {
            var linkDiv = document.getElementById("links")
            parsedData[key].forEach(function (item) {
                var newLink = document.createElement("div")
                newLink.innerHTML = `<a href="${item.link}" class="link d-flex p-2 m-2 rounded" style="background:  #${item.linkColor};" ${item.additionalAttributes != null ? item.additionalAttributes:""}><i class="${item.icon}" style="color:  #${item.iconColor};"></i><span class="text-center">${item.linkText}</span><span class="link-spacer"></span><span>${item.linkDescription}</span></a>`
                linkDiv.appendChild(newLink)
            })
        } else {
            document.getElementById(key).innerText = parsedData[key]
        }
    })
}