const ROLE_KEY = "role";
const NAME_KEY = "name";
const ID_KEY = "id";

const getItem = (key) => localStorage.getItem(key);

export const getRole = () => getItem(ROLE_KEY);
export const getName = () => getItem(NAME_KEY);
export const getId = () => getItem(ID_KEY);

export const saveClaimsToLocalStorage = (payload) => {
    localStorage.setItem(ROLE_KEY, mapRole(payload.userType));
    localStorage.setItem(NAME_KEY, payload.name);
    localStorage.setItem(ID_KEY, payload.userId);
    dispatchEvent(
        new CustomEvent("user-logged-in", {
            detail: {
                creds: [getRole(), getId()],
            },
        })
    );
    return { userType: getRole(), userId: payload.userId, name: payload.name };
};

const mapRole = (roleAsNum) => {
    return {
        0: "Registered",
        1: "VillaOwner",
        2: "BoatOwner",
        3: "Instructor",
        4: "Admin",
    }[roleAsNum];
};

export const clearStorage = () => localStorage.clear();
