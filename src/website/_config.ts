import lume from "lume/mod.ts";
import nav from "lume/plugins/nav.ts";

const site = lume({ src: "./src", prettyUrls: false });

site.use(nav());

site.add("_well-known", "_well-known");
site.add("img", "src/img");
site.add("css", "src/css");

export default site;
