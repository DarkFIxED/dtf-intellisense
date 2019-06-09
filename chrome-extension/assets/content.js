const commentsContainerSelector = "#page_wrapper .comments";
const unobservedCommentsTextAreaSelector = "#page_wrapper .comments textarea.comments_form__textarea:not(.meme-observed)";
const observedCommentsTextAreaSelector = "#page_wrapper .comments textarea.comments_form__textarea.meme-observed";

const serverUrl = "https://intellimemes.online/memes/search";
let serverVocabulary = undefined;

const wordsRegex = /[a-z0-9а-я]+/gm;

function findMatches(vocabulary, currentValue) {
    let matches = [];

    if (!vocabulary) {
        return {
            matches: matches,
            hasRelevantMatches: false
        };
    }

    currentValue = currentValue.toLowerCase();

    let words = currentValue.match(wordsRegex);

    let lexemesStatements = [];
    const maxLexemeStatementLength = 5;

    let deep = Math.max(0, words.length - maxLexemeStatementLength);
    for (let i = words.length; i >= deep; i--) {
        for (let j = 1; j <= Math.min(i, maxLexemeStatementLength); j++) {
            let lexemeStatement = words.slice(i - j, i).join(" ");
            lexemesStatements.push(lexemeStatement);
        }
    }

    vocabulary
        .filter(entry => words.length === 0
            ? true
            : entry.aliases.some(alias => lexemesStatements.some(lexeme => alias.search(lexeme) >= 0))
        )
        .map(entry => {
            matches.push({
                alias: entry.aliases[0],
                displayingName: entry.displayingName
            })
        });

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
