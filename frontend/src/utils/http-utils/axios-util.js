import axios from "axios";
import { initEndPointCreator } from "../path-loader.js";

export default function init(resource) {
    const createEndPoint = initEndPointCreator(resource);

    return {
        get: ({
            relPath = "",
            params = {},
            contentType = "application/json",
            responseType = "json",
            responseInfo = [],
        } = {}) =>
            request({
                url: createEndPoint(relPath),
                method: "GET",
                params,
                contentType,
                responseType,
                responseInfo,
            }),

        post: ({
            data,
            relPath = "",
            contentType = "application/json",
            responseType = "json",
            responseInfo = [],
        } = {}) =>
            request({
                url: createEndPoint(relPath),
                method: "POST",
                data: data,
                contentType,
                responseType,
                responseInfo,
            }),

        put: ({
            data,
            relPath = "",
            contentType = "application/json",
            responseType = "json",
            responseInfo = [],
        } = {}) =>
            request({
                url: createEndPoint(relPath),
                method: "PUT",
                data: data,
                contentType,
                responseType,
                responseInfo,
            }),

        delete: ({
            relPath = "",
            contentType = "application/json",
            responseType = "json",
            responseInfo = [],
        } = {}) =>
            request({
                url: createEndPoint(relPath),
                method: "DELETE",
                contentType,
                responseType,
                responseInfo,
            }),

        sendFileForm: ({
            rawFormData,
            relPath = "",
            responseType = "json",
            method = "POST",
            responseInfo = [],
        } = {}) =>
            request({
                url: createEndPoint(relPath),
                method,
                data: createFormData(rawFormData),
                contentType: "multipart/form-data",
                responseType,
                responseInfo,
            }),
    };
}

const request = async (config) => {
    const response = await axios({
        url: config.url,
        data: config.data,
        method: config.method,
        params: config.params,
        headers: {
            ...contentTypeHeader(config.contentType),
        },
        responseType: config.responseType,
        withCredentials: true,
    });

    const data =
        config.contentType === "blob"
            ? new Blob([response.data])
            : response.data;

    if (!config.responseInfo.lenght) return data;

    return config.responseInfo.reduce(
        (responseSubset, responseField) => {
            responseSubset[responseField] = response[responseField];
            return responseSubset;
        },
        { data } // always return data - other response fields are optional
    );
};

const contentTypeHeader = (contentType) => ({
    "Content-Type": `${contentType}`,
});

const JSONblobify = (obj) =>
    new Blob([JSON.stringify(obj)], { type: "application/json" });

// rawFormData = { objects: {obj1, obj2, ...}, files: {file1, file2, ...}}
const createFormData = (rawFormData) => {
    const formData = new FormData();
    Object.entries(rawFormData.objects).forEach(([key, value]) =>
        formData.append(key, JSONblobify(value))
    );
    Object.entries(rawFormData.files).forEach(([key, value]) =>
        formData.append(key, value)
    );
    return formData;
};
