// themeHandler.js - A resilient theme handler for Blazor
window.themeHandler = {
    // Initialize and set up the theme system
    initialize: function () {
        console.log("Theme handler initializing...");

        // Set the theme based on user preference or system preference
        this.applyTheme();

        // Set up listeners for theme changes
        this.setupListeners();

        // Update the UI to show the current theme
        this.updateUI();

        console.log("Theme handler initialization complete");
    },

    // Get the stored theme from localStorage
    getStoredTheme: function () {
        return localStorage.getItem('theme');
    },

    // Set the stored theme in localStorage
    setStoredTheme: function (theme) {
        localStorage.setItem('theme', theme);
    },

    // Get the preferred theme based on stored preference or system preference
    getPreferredTheme: function () {
        const storedTheme = this.getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    },

    // Apply the current theme to the document
    applyTheme: function () {
        const theme = this.getPreferredTheme();

        if (theme === 'auto') {
            document.documentElement.setAttribute(
                'data-bs-theme',
                window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
            );
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme);
        }

        console.log("Theme applied: " + theme);
    },

    // Update the UI to reflect the current theme
    updateUI: function () {
        try {
            const theme = this.getPreferredTheme();
            const themeSwitcher = document.querySelector('#bd-theme');

            if (!themeSwitcher) {
                console.log("Theme switcher element not found");
                return;
            }

            const themeSwitcherText = document.querySelector('#bd-theme-text');
            const activeThemeIcon = document.querySelector('.theme-icon-active use');
            const btnToActive = document.querySelector(`[data-bs-theme-value="${theme}"]`);

            if (!btnToActive) {
                console.log(`Button for theme ${theme} not found`);
                return;
            }

            const svgUseElement = btnToActive.querySelector('svg use');
            if (!svgUseElement) {
                console.log("SVG use element not found in theme button");
                return;
            }

            const svgOfActiveBtn = svgUseElement.getAttribute('href');

            // Update button states
            document.querySelectorAll('[data-bs-theme-value]').forEach(element => {
                element.classList.remove('active');
                element.setAttribute('aria-pressed', 'false');
            });

            btnToActive.classList.add('active');
            btnToActive.setAttribute('aria-pressed', 'true');

            if (activeThemeIcon && svgOfActiveBtn) {
                activeThemeIcon.setAttribute('href', svgOfActiveBtn);
            }

            if (themeSwitcherText) {
                const themeSwitcherLabel = `${themeSwitcherText.textContent} (${btnToActive.dataset.bsThemeValue})`;
                themeSwitcher.setAttribute('aria-label', themeSwitcherLabel);
            }

            console.log("UI updated for theme: " + theme);
        } catch (error) {
            console.error("Error updating theme UI: ", error);
        }
    },

    // Set up event listeners for theme changes
    setupListeners: function () {
        // Listen for system preference changes
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
            const storedTheme = this.getStoredTheme();
            if (storedTheme !== 'light' && storedTheme !== 'dark') {
                this.applyTheme();
                this.updateUI();
            }
        });

        // Listen for manual theme changes
        document.addEventListener('click', (event) => {
            let target = event.target;

            // Search up to 3 levels for the theme value attribute
            for (let i = 0; i < 3; i++) {
                if (!target) break;

                if (target.hasAttribute('data-bs-theme-value')) {
                    const theme = target.getAttribute('data-bs-theme-value');
                    this.setStoredTheme(theme);
                    this.applyTheme();
                    this.updateUI();
                    break;
                }

                target = target.parentElement;
            }
        });

        // Additional listener for Blazor navigation events
        document.addEventListener('DOMContentLoaded', () => {
            this.applyTheme();
            this.updateUI();
        });

        // MutationObserver to watch for changes to the DOM
        const observer = new MutationObserver((mutations) => {
            let shouldUpdate = false;

            for (const mutation of mutations) {
                if (mutation.type === 'childList' &&
                    mutation.addedNodes.length > 0 &&
                    Array.from(mutation.addedNodes).some(node =>
                        node.nodeType === 1 && (
                            node.id === 'bd-theme' ||
                            node.querySelector && node.querySelector('#bd-theme')
                        )
                    )) {
                    shouldUpdate = true;
                    break;
                }
            }

            if (shouldUpdate) {
                setTimeout(() => {
                    this.updateUI();
                }, 50);
            }
        });

        // Start observing the document body for changes
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });

        console.log("Theme listeners set up");
    },

    // Force an update of the theme (can be called from Blazor)
    forceUpdate: function () {
        this.applyTheme();
        this.updateUI();
        return true;
    }
};

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    window.themeHandler.initialize();
});

// Ensure theme is applied even if the DOM is already loaded
if (document.readyState === 'complete' || document.readyState === 'interactive') {
    window.themeHandler.initialize();
}