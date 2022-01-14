import { getRole } from "../utils/local-storage-util.js";

export default {
    data() {
        return {
            role: getRole(),
        };
    },

    computed: {
        isGuest() {
            return !this.role;
        },
        isRegistered() {
            return this.role === "Registered";
        },
        isAdmin() {
            return this.role === "Admin";
        },
        isVillaOwner() {
            return this.role === "VillaOwner";
        },
        isBoatOwner() {
            return this.role === "BoatOwner";
        },
        isInstructor() {
            return this.role === "Instructor";
        },
    },
};
