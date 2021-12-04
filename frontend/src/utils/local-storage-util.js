const ROLE_KEY = 'role';
const NAME_KEY = 'name';
const ID_KEY = 'id';

const getItem = (key) => localStorage.getItem(key);

export const getRole = () => getItem(ROLE_KEY);
export const getName = () => getItem(NAME_KEY);
export const getId = () => getItem(ID_KEY);

export const saveClaimsToLocalStorage = (payload) => {
    localStorage.setItem(ROLE_KEY, payload.role);
    localStorage.setItem(NAME_KEY, payload.name);
    localStorage.setItem(ID_KEY, payload.id);
    dispatchEvent(new CustomEvent('user-logged-in', {
        detail: {
          creds: [
            getRole(),
            getId()
          ]
        }
    }));
}

export const clearStorage = () => localStorage.clear();