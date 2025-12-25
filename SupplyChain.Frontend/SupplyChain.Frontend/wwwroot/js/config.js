
const AppConfig = {

    api: {
        baseUrl: 'http://localhost:5256',
        timeout: 30000
    },

    session: {
        inactivityTimeout: 30 * 60 * 1000,
        sessionKey: 'silsila_session'
    },

    ui: {
        toastDuration: 5000,
        searchDebounce: 300,
        animationDuration: 300,
        chartUpdateInterval: 3000
    },

    notifications: {
        storageKey: 'silsila_notifications',
        demoInterval: 30000,
        demoProbability: 0.7
    },

    table: {
        defaultRowsPerPage: 10,
        maxErrorLogs: 50
    },

    avatar: {
        baseUrl: 'https://ui-avatars.com/api/',
        defaultBackground: '10b981',
        defaultColor: 'fff'
    },

    demo: {
        enabled: true,
        toastDelay: 5000,
        toastInterval: 10000,
        toastProbability: 0.6
    }
};

if (typeof module !== 'undefined' && module.exports) {
    module.exports = AppConfig;
}
