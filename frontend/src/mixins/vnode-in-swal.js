import { defineCustomElement } from "vue";

export default {
    data() {
        return {
            renderedComponent: null,
        };
    },
    methods: {
        showComponent(component, propsData, swalOptions, swalCallback) {
            this.$swal
                .fire({
                    ...swalOptions,
                    html: `<div id="SwalMountedComponent"></div>`,
                    willOpen: () => {
                        this.mountVueComponent(component, propsData);
                    },
                })
                .then((swalResult) => {
                    const componentResult = this.destroyVueComponent();
                    swalCallback(componentResult, swalResult);
                });
        },

        mountVueComponent(component, props) {
            let SwalComponent = customElements.get("swal-component");
            if (!SwalComponent) {
                SwalComponent = defineCustomElement(component);
                customElements.define("swal-component", SwalComponent);
            }
            const SwalComponentInstance = new SwalComponent({ ...props });

            document
                .getElementById("SwalMountedComponent")
                .appendChild(SwalComponentInstance); // this is refering to parent component

            this.renderedComponent = SwalComponentInstance;
        },
        destroyVueComponent() {
            const result = this.renderedComponent._instance.ctx.getResult();
            // programatically destroy component ?
            this.renderedComponent = null;
            return result;
        },
    },
};
