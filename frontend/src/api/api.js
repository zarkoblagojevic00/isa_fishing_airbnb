import axios from 'axios';

let axiosInstance = axios.create({ baseURL: 'https://localhost:44383', headers: { 'Set-Cookie': document.cookie } });

export default axiosInstance;