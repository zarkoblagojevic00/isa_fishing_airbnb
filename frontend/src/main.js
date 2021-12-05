import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import "viewerjs/dist/viewer.css";
import VueViewer from "v-viewer";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
    faPlus,
    faTrash,
    faEdit,
    faSearch,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import VCalendar from "v-calendar";
import jQuery from "jquery";

library.add(faTrash);
library.add(faEdit);
library.add(faPlus);
library.add(faSearch);

const $ = jQuery;
window.$ = $;

const app = createApp(App).component("font-awesome-icon", FontAwesomeIcon);
app.use(VueViewer);
app.use(VCalendar, {});
app.use(router).mount("#app");

if (process.env.VUE_APP_ENV == "development") {
    document.cookie = "userId=2; domain=localhost";
    document.cookie = "email=testvillaowner@gmail.com; domain=localhost";
}
