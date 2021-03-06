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
    faInfoCircle,
    faPlusCircle,
    faMapMarkerAlt,
    faSortUp,
    faSortDown,
    faCalendarCheck,
} from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import VCalendar from "v-calendar";
import jQuery from "jquery";
import axios from "axios";
import VueAxios from "vue-axios";
import VueSweetalert2 from "vue-sweetalert2";
import "sweetalert2/dist/sweetalert2.min.css";
import OpenLayersMap from "vue3-openlayers";
import "vue3-openlayers/dist/vue3-openlayers.css";

library.add(faTrash);
library.add(faEdit);
library.add(faPlus);
library.add(faSearch);
library.add(faInfoCircle);
library.add(faPlusCircle);
library.add(faMapMarkerAlt);
library.add(faSortUp);
library.add(faSortDown);
library.add(faCalendarCheck);
const $ = jQuery;
window.$ = $;

const app = createApp(App).component("font-awesome-icon", FontAwesomeIcon);
app.use(VueViewer);

app.use(OpenLayersMap);

app.use(VCalendar, {});
app.use(VueAxios, axios);
app.use(VueSweetalert2);
app.use(router).mount("#app");

if (process.env.VUE_APP_ENV == "development") {
    if (process.env.VUE_APP_USER == "mili") {
        //document.cookie = "userId=2; domain=localhost";
        //document.cookie = "email=testvillaowner@gmail.com; domain=localhost";
    } else if (process.env.VUE_APP_USER == "suki") {
        //document.cookie = "userId=4; domain=localhost";
        //document.cookie = "email=stef@gmail.com; domain=localhost";
    }
}
