export const parseProperty = (object, path) => {
    let copyPath = path;
    let copyObject = object;
    copyPath = copyPath.replace(/\[(\w+)\]/g, ".$1"); // convert indexes to properties
    copyPath = copyPath.replace(/^\./, ""); // strip a leading dot
    var a = copyPath.split(".");
    for (var i = 0, n = a.length; i < n; ++i) {
        var k = a[i];
        if (k in copyObject) {
            copyObject = copyObject[k];
        } else {
            return;
        }
    }
    return copyObject;
};
