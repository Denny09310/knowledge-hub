let editor;
let controller = new AbortController();

export function onUpdate() {
    controller.abort();

    initializeEditor();
    listenFormSubmit();
}

function initializeEditor() {
    const element = document.querySelector('#markdown-editor');
    if (!element) return;

    editor = new EasyMDE({
        element,
        uploadImage: true,
        sideBySideFullscreen: false,
        imageUploadEndpoint: '/Articles/Image',
        hideIcons: ['fullscreen']
    });
}

function listenFormSubmit() {
    const form = document.querySelector('form');
    if (!form) return;
    form.addEventListener('submit', () => {
        const markdown = editor.value();
        const input = document.querySelector('input[name="Content"]');
        if (input) {
            input.value = markdown;
        }
    }, { signal: controller.signal });
}