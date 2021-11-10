import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import "viewerjs/dist/viewer.css";
import VueViewer from "v-viewer";

const app = createApp(App);
app.use(VueViewer);
app.use(router).mount("#app");
