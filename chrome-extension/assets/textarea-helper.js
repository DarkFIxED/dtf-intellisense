var properties = [
    'boxSizing',
    'width',
    'height',
    'overflowX',
    'overflowY',

    'borderTopWidth',
    'borderRightWidth',
    'borderBottomWidth',
    'borderLeftWidth',

    'paddingTop',
    'paddingRight',
    'paddingBottom',
    'paddingLeft',

    'fontStyle',
    'fontVariant',
    'fontWeight',
    'fontStretch',
    'fontSize',
    'lineHeight',
    'fontFamily',

    'textAlign',
    'textTransform',
    'textIndent',
    'textDecoration',

    'letterSpacing',
    'wordSpacing'
];

var mirrorDiv, computed, style;

getCaretCoordinates = function (element) {
    let position = element.selectionEnd;
    // mirrored div
    mirrorDiv = document.getElementById(element.nodeName + '--mirror-div');
    if (!mirrorDiv) {
        mirrorDiv = document.createElement('div');
        mirrorDiv.id = element.nodeName + '--mirror-div';
        document.body.appendChild(mirrorDiv);
    }

    style = mirrorDiv.style;
    computed = getComputedStyle(element);

    // default textarea styles
    style.whiteSpace = 'pre-wrap';
    if (element.nodeName !== 'INPUT')
        style.wordWrap = 'break-word';  // only for textarea-s

    // position off-screen
    style.position = 'absolute';  // required to return coordinates properly
    style.top = element.offsetTop + parseInt(computed.borderTopWidth) + 'px';
    style.left = "400px";
    style.visibility = 'hidden';  // not 'display: none' because we want rendering

    // transfer the element's properties to the div
    properties.forEach(function (prop) {
        style[prop] = computed[prop];
    });


    style.overflow = 'hidden';  // for Chrome to not render a scrollbar; IE keeps overflowY = 'scroll'

    mirrorDiv.textContent = element.value.substring(0, position);
    // the second special handling for input type="text" vs textarea: spaces need to be replaced with non-breaking spaces - http://stackoverflow.com/a/13402035/1269037
    if (element.nodeName === 'INPUT')
        mirrorDiv.textContent = mirrorDiv.textContent.replace(/\s/g, "\u00a0");

    var span = document.createElement('span');
    // Wrapping must be replicated *exactly*, including when a long word gets
    // onto the next line, with whitespace at the end of the line before (#7).
    // The  *only* reliable way to do that is to copy the *entire* rest of the
    // textarea's content into the <span> created at the caret position.
    // for inputs, just '.' would be enough, but why bother?
    span.textContent = element.value.substring(position) || '.';  // || because a completely empty faux span doesn't render at all
    span.style.backgroundColor = "lightgrey";
    mirrorDiv.appendChild(span);

    var coordinates = {
        top: span.offsetTop + parseInt(computed['borderTopWidth']),
        left: span.offsetLeft + parseInt(computed['borderLeftWidth'])
    };

    return coordinates;
};
