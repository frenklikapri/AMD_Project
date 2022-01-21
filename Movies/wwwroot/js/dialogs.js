export function showModalWithId(modalId) {
    $(`#${modalId}`).modal("show")
}

export function hideModalWithId(modalId) {
    $(`#${modalId}`).modal("hide")
}