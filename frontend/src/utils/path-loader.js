const api_root = "api";

const root_prod = `/${api_root}/`;
const root_dev = `${process.env.VUE_APP_BASE_API_URL}${api_root}/`;

const isDevMode = () => location.port.startsWith("8");

export const getRoot = () => (isDevMode() ? root_dev : root_prod);

// for requests
export const initEndPointCreator = (resource) => (relPath) =>
    `${getRoot()}${resource}/${relPath}`;
