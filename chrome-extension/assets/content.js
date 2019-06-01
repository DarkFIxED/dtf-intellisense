const commentsContainerSelector = "#page_wrapper .comments";
const unobservedCommentsTextAreaSelector = "#page_wrapper .comments textarea.comments_form__textarea:not(.meme-observed)";
const observedCommentsTextAreaSelector = "#page_wrapper .comments textarea.comments_form__textarea.meme-observed";

function processSymbolKey(target, keyboardEvent) {
    let currentValue = target.value;
    if (keyboardEvent.key.length === 1) {
        currentValue += keyboardEvent.key;
    }

    setToolTipContent(target, currentValue);
    openToolTip(target);
}

function setToolTipContent(target, value) {
    $(target).tooltipster('content', value);
}

function processCommandKey(target, keyboardEvent) {

    // Esc code
    if (keyboardEvent.keyCode === 27) {
        closeToolTip(target);
    }

    // Backspace code
    if (keyboardEvent.keyCode === 8) {
        let value = target.value.length > 0
            ? target.value.substr(0, target.value.length - 1)
            : target.value;
        
        setToolTipContent(target, value);
    }

    if (keyboardEvent.repeat) {
        return;
    }

    if (keyboardEvent.ctrlKey && keyboardEvent.key === " ") {
        openToolTip(target);
    }
}

function initInputListener(e) {
    let target = e.target;
    let keyboardEvent = e.originalEvent;

    if (keyboardEvent.key.length === 1 && !keyboardEvent.altKey && !keyboardEvent.ctrlKey) {
        processSymbolKey(target, keyboardEvent);
    } else {
        processCommandKey(target, keyboardEvent);
    }
}

function openToolTip(target) {
    $(target).tooltipster('open');
    target['meme'].opened = true;
}

function closeToolTip(target) {
    $(target).tooltipster('close');
    target['meme'].opened = false;
}

function initializeToolTip(textArea) {
    textArea['meme'] = {
        opened: false
    };

    $(textArea).tooltipster({
        interactive: true,
        selfDestruction: false,
        contentCloning: true,
        trigger: "custom"
    });
}

function initializeEventListeners(target) {
    $(target).bind("keydown", function (event) {
        initInputListener(event);
    });

    $(target).bind("blur", function () {
        closeToolTip(target);
    });
}

function subscribeToDOMChange() {
    $(commentsContainerSelector).bind("DOMSubtreeModified", function () {
        let textAreas = $(unobservedCommentsTextAreaSelector);

        for (let textArea of textAreas) {
            $(textArea).addClass("meme-observed");

            initializeEventListeners(textArea);
            initializeToolTip(textArea);
        }
    });
}

$(commentsContainerSelector).ready(function () {
    subscribeToDOMChange();
});
