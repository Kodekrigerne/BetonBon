window.storage = {
    save: (key, value) => localStorage.setItem(key, value),
    load: (key) => localStorage.getItem(key),
    remove: (key) => localStorage.removeItem(key)
};
