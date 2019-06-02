const commentsContainerSelector = "#page_wrapper .comments";
const unobservedCommentsTextAreaSelector = "#page_wrapper .comments textarea.comments_form__textarea:not(.meme-observed)";
const observedCommentsTextAreaSelector = "#page_wrapper .comments textarea.comments_form__textarea.meme-observed";
const memeListId = "meme-select";
const memeListSelector = "#meme-select";
const serverUrl = "https://intellimemes.online/memes/search";

let serverVocabulary = undefined;

function buildContent(options) {
    if (options.length === 0) {
        return `No matches found`;
    }

    // language=HTML
    let result = `<select id="${memeListId}" size="5">`;
    for (let option of options) {
        result = result + `<option value="${option.alias}" ${options.indexOf(option) === 0 ? "selected" : ""}>${option.displayingName}&nbsp;(${option.alias})</option>`;
    }

    result = result + "</select>";
    return result;
}

function findMatches(vocabulary, currentValue) {
    let matches = [];

    if (!vocabulary) {
        return {
            matches: matches,
            hasRelevantMatches: false
        };
    }

    let words = currentValue.split(" ");
    let lastWord = words[words.length - 1];

    vocabulary
        .filter(entry => words.length === 0
            ? true
            : entry.aliases.some(alias => alias.search(lastWord) >= 0
            ))
        .map(entry => entry.aliases.map(alias => {
            matches.push({
                alias: alias,
                displayingName: entry.displayingName
            });
        }));

    return {
        matches: matches,
        hasRelevantMatches: words.length !== 0 && matches.length > 0
    };
}

function processSymbolKey(target, keyboardEvent) {
    let currentValue = target.value;
    if (keyboardEvent.key.length === 1) {
        currentValue += keyboardEvent.key;
    }

    if (setToolTipContent(target, currentValue).hasRelevantMatches) {
        openToolTip(target);
    } else {
        closeToolTip(target);
    }
}

function setToolTipContent(target, value) {
    let matches = findMatches(serverVocabulary, value);
    let content = buildContent(matches.matches);
    $(target).tooltipster('content', content);

    return {
        hasRelevantMatches: matches.hasRelevantMatches,
        optionsCount: content.length
    };
}

function selectPrevMatch() {
    let currentValue = $(memeListSelector).val();
    let allValues = [];

    let options = $(`${memeListSelector} option`);
    options.each(option => {
        allValues.push($(options[option]).attr('value'))
    });

    let currentIndex = allValues.indexOf(currentValue);
    let newIndex = currentIndex === 0
        ? allValues.length - 1
        : currentIndex - 1;

    let newValue = allValues[newIndex];
    $(memeListSelector).val(newValue);
}

function selectNextMatch() {
    let currentValue = $(memeListSelector).val();
    let allValues = [];

    let options = $(`${memeListSelector} option`);
    options.each(option => {
        allValues.push($(options[option]).attr('value'))
    });

    let currentIndex = allValues.indexOf(currentValue);
    let newIndex = currentIndex === allValues.length - 1
        ? 0
        : currentIndex + 1;

    let newValue = allValues[newIndex];
    $(memeListSelector).val(newValue);
}

function insertCurrent(target) {
    let currentValue = `\${${$(memeListSelector).val()}}`;
    let targetValue = $(target).val();

    let words = targetValue.split(" ");
    if (words.length === 0) {
        $(target).val(currentValue);
        return;
    }

    let lastWord = words[words.length - 1];
    let lastEntryPointIndex = targetValue.lastIndexOf(lastWord);
    let newValue = targetValue.substr(0, lastEntryPointIndex).concat(currentValue);
    $(target).val(newValue);
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

    if (target['meme'].opened) {
        switch (keyboardEvent.keyCode) {

            // arrow up
            case 38:
                selectPrevMatch();
                return false;

            // arrow down
            case 40:
                selectNextMatch();
                return false;

            // enter
            case 13:
                insertCurrent(target);
                closeToolTip(target);
                return false;
        }
    }

    // Skip repeats
    if (keyboardEvent.repeat) {
        return true;
    }

    // Force show on Ctrl+Space
    if (keyboardEvent.ctrlKey && keyboardEvent.key === " ") {
        setToolTipContent(target, target.value);
        openToolTip(target);
    }

    return true;
}

function initInputListener(e) {
    let target = e.target;
    let keyboardEvent = e.originalEvent;

    if (keyboardEvent.key.length === 1 && !keyboardEvent.altKey && !keyboardEvent.ctrlKey) {
        processSymbolKey(target, keyboardEvent);
    } else {
        if (!processCommandKey(target, keyboardEvent)) {
            e.preventDefault();
        }
    }
}

function openToolTip(target) {
    $(target).tooltipster('open');

    $(memeListSelector).bind("keydown", function (event) {
        if (event.originalEvent.keyCode === 13) {
            insertCurrent(target);
            closeToolTip(target);
        }
    });

    $(memeListSelector).bind("dblclick", function (event) {
        insertCurrent(target);
        closeToolTip(target);
    });

    target['meme'].opened = true;
}

function closeToolTip(target) {
    $(memeListSelector).unbind();
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
        trigger: "custom",
        contentAsHTML: true,
    });
}

function initializeEventListeners(target) {
    $(target).bind("keydown", function (event) {
        initInputListener(event);
    });

    $(target).bind("blur", function () {
        //closeToolTip(target);
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

function loadPatterns() {
    $.getJSON(serverUrl, function (response) {
        if (!response.isSuccess) {
            throw new Error("Server request fault");
        }

        serverVocabulary = response.data;
    });
}

$(commentsContainerSelector).ready(function () {
    subscribeToDOMChange();
    loadPatterns();
});
