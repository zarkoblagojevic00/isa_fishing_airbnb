import axios from "axios";
import { initEndPointCreator } from "../path-loader.js";

export default function init(resource) {
  const createEndPoint = initEndPointCreator(resource);

  return {
    get: ({
      relPath = "",
      contentType = "application/json",
      responseType = "json",
      params = {},
    } = {}) =>
      request({
        url: createEndPoint(relPath),
        method: "GET",
        params,
        contentType,
        responseType,
      }),

    post: ({
      data,
      relPath = "",
      contentType = "application/json",
      responseType = "json",
    } = {}) =>
      request({
        url: createEndPoint(relPath),
        method: "POST",
        data: data,
        contentType,
        responseType,
      }),

    put: ({
      data,
      relPath = "",
      contentType = "application/json",
      responseType = "json",
    } = {}) =>
      request({
        url: createEndPoint(relPath),
        method: "PUT",
        data: data,
        contentType,
        responseType,
      }),

    delete: ({
      relPath = "",
      contentType = "application/json",
      responseType = "json",
    } = {}) =>
      request({
        url: createEndPoint(relPath),
        method: "DELETE",
        contentType,
        responseType,
      }),

    sendFileForm: ({
      rawFormData,
      relPath = "",
      responseType = "json",
      method = "POST",
    } = {}) =>
      request({
        url: createEndPoint(relPath),
        method,
        data: createFormData(rawFormData),
        contentType: "multipart/form-data",
        responseType,
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
      ...setCookieHeader,
    },
    responseType: config.responseType,
  });

  // TODO: Change to work with new image format
  return config.contentType === "blob"
    ? new Blob([response.data])
    : response.data;
};

const contentTypeHeader = (contentType) => ({
  "Content-Type": `${contentType}`,
});
const setCookieHeader = { "Set-Cookie": document.cookie };

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
