import axios from "axios";

const isDevMode = () => process.env.VUE_APP_ENV == "development";

let axiosInstance = axios.create({
    baseURL: isDevMode() ? "https://localhost:44383" : "",
    headers: {
        "Set-Cookie": document.cookie,
    },
});

export default axiosInstance;
