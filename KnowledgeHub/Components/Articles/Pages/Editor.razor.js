export function onUpdate() {
    initializeEditor();
}

function initializeEditor() {
    const element = document.querySelector('#markdown-editor');
    if (!element) return;

    const editor = new EasyMDE({
        element,
        uploadImage: true,
        sideBySideFullscreen: false,
        imageUploadEndpoint: '/Articles/Image',
        hideIcons: ['fullscreen']
    });

    console.debug(editor)
}