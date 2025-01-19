export function onUpdate() {
    initializeFeatures();
}

// Initializes features like anchor links, clipboard functionality, and syntax highlighting
function initializeFeatures() {
    addHeadingAnchors();
    enableClipboardButtons();
    Prism.highlightAll(); 
}

// Adds anchor links to heading elements for easier navigation
function addHeadingAnchors() {
    const headingSelectors = [
        'h2',
        'h3',
        'h4',
        'h5',
        'h6',
    ];
    anchors.add(headingSelectors.join(','));
}

// Enables clipboard copy functionality for all code blocks
function enableClipboardButtons() {
    const codeBlocks = document.querySelectorAll('pre');

    codeBlocks.forEach((codeBlock) => {
        const copyButton = createCopyButton();
        codeBlock.appendChild(copyButton);
    });

    initializeClipboard();
}

// Creates a "Copy" button element for code blocks
function createCopyButton() {
    const button = document.createElement('button');
    button.className = 'copy-button relative group';
    button.type = 'button';
    button.innerHTML = getCopyButtonSVG();
    button.onclick = handleCopyButtonClick;

    // Screen-reader support
    const buttonText = document.createElement('span');
    buttonText.className = 'sr-only';
    buttonText.textContent = 'Copy to clipboard';
    button.appendChild(buttonText);

    return button;
}

// Provides feedback to the user after the copy button is clicked
function handleCopyButtonClick() {
    const button = this;
    const icon = button.querySelector('svg');

    // Change icon to green checkmark
    icon.innerHTML = getCheckmarkSVG();
    icon.classList.add('text-green-500');

    // Reset state after 1 second
    setTimeout(() => {
        icon.innerHTML = getCopyButtonSVG();
        icon.classList.remove('text-green-500');
    }, 1000);
}

// SVG icon for the "Copy" button
function getCopyButtonSVG() {
    return `
    <svg class="w-4 h-4 text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
      <path stroke="currentColor" stroke-linejoin="round" stroke-width="2" d="M9 8v3a1 1 0 0 1-1 1H5m11 4h2a1 1 0 0 0 1-1V5a1 1 0 0 0-1-1h-7a1 1 0 0 0-1 1v1m4 3v10a1 1 0 0 1-1 1H6a1 1 0 0 1-1-1v-7.13a1 1 0 0 1 .24-.65L7.7 8.35A1 1 0 0 1 8.46 8H13a1 1 0 0 1 1 1Z"/>
    </svg>
  `;
}

// SVG icon for the green checkmark
function getCheckmarkSVG() {
    return `
    <svg class="w-4 h-4 text-green-500" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
      <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
    </svg>
  `;
}

// Initializes clipboard.js for the copy buttons
function initializeClipboard() {
    const clipboard = new ClipboardJS('.copy-button', {
        target: (trigger) => trigger.previousElementSibling,
    });

    clipboard.on('success', (event) => {
        event.clearSelection();
    });
}
