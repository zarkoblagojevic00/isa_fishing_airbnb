import { getRoot } from "../utils/path-loader.js";

export default {
    methods: {
        getImageStyle(id) {
            return {
                backgroundImage: `url('${getRoot()}Image/GetImage?id=${id}')`,
                backgroundSize: "cover",
                backgroundPosition: "center",
            };
        },
    },
};
