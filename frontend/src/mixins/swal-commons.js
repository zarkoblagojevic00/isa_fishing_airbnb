export default {
    data() {
        return {
            toast: this.$swal.mixin({
                toast: true,
                position: "top-right",
                iconColor: "white",
                customClass: {
                    popup: "colored-toast",
                },
                showConfirmButton: false,
                timer: 4000,
            }),
            bookSetupObject: {
                showCancelButton: true,
                confirmButtonText: "Book",
                cancelButtonText: "Cancel",
                showCloseButton: true,
                showLoaderOnConfirm: true,
                buttonsStyling: false,
                heightAuto: false,
                allowOutsideClick: () => !this.$swal.isLoading(),
                showClass: {
                    popup: "animated fadeInDown",
                },
                hideClass: {
                    popup: "animated fadeOut faster",
                },
                customClass: {
                    popup: "modal-popup",
                    htmlContainer: "modal-content",
                    actions: "modal-actions",
                    confirmButton:
                        "clickable primary transition-ease modal-button",
                    cancelButton:
                        "clickable danger transition-ease modal-button",
                },
            },
            sendSetupObject: {
                showCancelButton: true,
                confirmButtonText: "Send",
                cancelButtonText: "Cancel",
                showCloseButton: true,
                showLoaderOnConfirm: true,
                buttonsStyling: false,
                heightAuto: false,
                allowOutsideClick: () => !this.$swal.isLoading(),
                showClass: {
                    popup: "animated fadeInDown",
                },
                hideClass: {
                    popup: "animated fadeOut faster",
                },
                customClass: {
                    popup: "modal-popup",
                    htmlContainer: "modal-content",
                    actions: "modal-actions",
                    confirmButton:
                        "clickable primary transition-ease modal-button",
                    cancelButton:
                        "clickable danger transition-ease modal-button",
                },
            },
        };
    },
};
