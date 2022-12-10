/*!
  * PARTLY FROM: 
  * Bootstrap v5.2.3 (https://getbootstrap.com/)
  * Copyright 2011-2022 The Bootstrap Authors (https://github.com/twbs/bootstrap/graphs/contributors)
  * Licensed under MIT (https://github.com/twbs/bootstrap/blob/main/LICENSE)
  */
(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? module.exports = factory() :
        typeof define === 'function' && define.amd ? define(factory) :
            (global = typeof globalThis !== 'undefined' ? globalThis : global || self, global.bootstrap = factory());
})(this, (function () {
    'use strict';

    /**
     * --------------------------------------------------------------------------
     * Bootstrap (v5.2.3): util/index.js
     * Licensed under MIT (https://github.com/twbs/bootstrap/blob/main/LICENSE)
     * --------------------------------------------------------------------------
     */
    const MAX_UID = 1000000;
    const MILLISECONDS_MULTIPLIER = 1000;
    const TRANSITION_END = 'transitionend'; // Shout-out Angus Croll (https://goo.gl/pxwQGp)

    const toType = object => {
        if (object === null || object === undefined) {
            return `${object}`;
        }

        return Object.prototype.toString.call(object).match(/\s([a-z]+)/i)[1].toLowerCase();
    };
    /**
     * Public Util API
     */


    const getUID = prefix => {
        do {
            prefix += Math.floor(Math.random() * MAX_UID);
        } while (document.getElementById(prefix));

        return prefix;
    };

    const getSelector = element => {
        let selector = element.getAttribute('data-bs-target');

        if (!selector || selector === '#') {
            let hrefAttribute = element.getAttribute('href'); // The only valid content that could double as a selector are IDs or classes,
            // so everything starting with `#` or `.`. If a "real" URL is used as the selector,
            // `document.querySelector` will rightfully complain it is invalid.
            // See https://github.com/twbs/bootstrap/issues/32273

            if (!hrefAttribute || !hrefAttribute.includes('#') && !hrefAttribute.startsWith('.')) {
                return null;
            } // Just in case some CMS puts out a full URL with the anchor appended


            if (hrefAttribute.includes('#') && !hrefAttribute.startsWith('#')) {
                hrefAttribute = `#${hrefAttribute.split('#')[1]}`;
            }

            selector = hrefAttribute && hrefAttribute !== '#' ? hrefAttribute.trim() : null;
        }

        return selector;
    };

    const getSelectorFromElement = element => {
        const selector = getSelector(element);

        if (selector) {
            return document.querySelector(selector) ? selector : null;
        }

        return null;
    };

    const getElementFromSelector = element => {
        const selector = getSelector(element);
        return selector ? document.querySelector(selector) : null;
    };

    const getTransitionDurationFromElement = element => {
        if (!element) {
            return 0;
        } // Get transition-duration of the element


        let {
            transitionDuration,
            transitionDelay
        } = window.getComputedStyle(element);
        const floatTransitionDuration = Number.parseFloat(transitionDuration);
        const floatTransitionDelay = Number.parseFloat(transitionDelay); // Return 0 if element or transition duration is not found

        if (!floatTransitionDuration && !floatTransitionDelay) {
            return 0;
        } // If multiple durations are defined, take the first


        transitionDuration = transitionDuration.split(',')[0];
        transitionDelay = transitionDelay.split(',')[0];
        return (Number.parseFloat(transitionDuration) + Number.parseFloat(transitionDelay)) * MILLISECONDS_MULTIPLIER;
    };

    const triggerTransitionEnd = element => {
        element.dispatchEvent(new Event(TRANSITION_END));
    };

    const isElement$1 = object => {
        if (!object || typeof object !== 'object') {
            return false;
        }

        if (typeof object.jquery !== 'undefined') {
            object = object[0];
        }

        return typeof object.nodeType !== 'undefined';
    };
