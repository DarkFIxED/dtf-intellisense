const memeListId = "meme-select";
const memeListSelector = "#meme-select";

function setToolTipContent(target, value) {
    let matches = findMatches(serverVocabulary, value);
    let content = buildToolTipContent(matches.matches);
    $(target).tooltipster('content', content);

    return {
        hasRelevantMatches: matches.hasRelevantMatches,
        optionsCount: content.length
    };
}

function getCurrentToolTipValue() {
    return $(`${memeListSelector} .meme-item[selected]`).attr('value');
}

function getAllToolTipValues() {
    let allValues = [];

    let options = $(`${memeListSelector} .meme-item`);
    options.each(option => {
        allValues.push($(options[option]).attr('value'))
    });

    return allValues;
}

function setToolTIpValue(value) {
    let options = $(`${memeListSelector} .meme-item`);
    options.each(optionIndex => {
        let option = $(options[optionIndex]);
        if (option.attr('value') === value) {
            option.attr('selected', '');
            option.get()[0].scrollIntoViewIfNeeded(false);
        } else {
            option.removeAttr('selected');
        }
    });
}

function selectPrevMatch() {
    let currentValue = getCurrentToolTipValue();
    let allValues = getAllToolTipValues();

    let currentIndex = allValues.indexOf(currentValue);
    let newIndex = currentIndex === 0
        ? allValues.length - 1
        : currentIndex - 1;

    let newValue = allValues[newIndex];
    setToolTIpValue(newValue);
}

function selectNextMatch() {
    let currentValue = getCurrentToolTipValue();
    let allValues = getAllToolTipValues();

    let currentIndex = allValues.indexOf(currentValue);
    let newIndex = currentIndex === allValues.length - 1
        ? 0
        : currentIndex + 1;

    let newValue = allValues[newIndex];
    setToolTIpValue(newValue);
}

function insertCurrent(target) {
    let currentValue = `\${${getCurrentToolTipValue()}}`;
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


function openToolTip(target) {
    $(target).tooltipster('open');

    $(memeListSelector).bind("keydown", function (event) {
        if (event.originalEvent.keyCode === 13) {
            insertCurrent(target);
            closeToolTip(target);
        }
    });

    $(memeListSelector).bind("click", function (event) {
        setToolTIpValue(event.target.getAttribute('value'));
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
        functionPosition: function(instance, helper, position){
            position.side = "bottom";
            position.distance = 0;

            let caretCoords = getCaretCoordinates(textArea);
            console.log(caretCoords);

            position.coord.top = helper.geo.origin.windowOffset.top + caretCoords.top + 20;
            position.coord.left = helper.geo.origin.windowOffset.left + caretCoords.left - 6;
            position.target = caretCoords.left;

            return position;
        }
    });
}

function buildToolTipContent(options) {
    if (options.length === 0) {
        return `No matches found`;
    }

    let result = `<div id="${memeListId}">\n\r`;
    for (let option of options) {
        result = result + `<div class="meme-item" value="${option.alias}" ${options.indexOf(option) === 0 ? "selected" : ""}>${option.displayingName}&nbsp;(${option.alias})</div>\n\r`;
    }
    result = result + "</div>";

    return result;
}
