import { createRouter, createWebHistory } from "vue-router";
import Gallery from "../views/Gallery.vue";
import AdventureDetails from "../views/AdventureDetails.vue";
import AdventureProfile from "../views/AdventureProfile.vue";
import Homepage from "../views/Homepage.vue";

const routes = [
  {
    path: "/adventure/gallery",
    name: "Gallery",
    component: Gallery,
  },
  {
    path: "/adventure/details",
    name: "Details",
    component: AdventureDetails,
  },
  {
    path: "/adventure",
    name: "AdventureProfile",
    component: AdventureProfile,
  },
  {
    path: "/",
    name: "Homepage",
    component: Homepage,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
