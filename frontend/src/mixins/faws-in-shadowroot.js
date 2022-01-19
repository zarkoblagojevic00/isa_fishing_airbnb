import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { config, dom } from "@fortawesome/fontawesome-svg-core";

export default {
    components: {
        FontAwesomeIcon,
    },
    mounted() {
        config.autoAddCss = false;
        const shadowRoot = this.$el.parentNode;
        const id = "fa-styles";

        if (!shadowRoot.getElementById(`${id}`)) {
            const faStyles = document.createElement("style");
            faStyles.setAttribute("id", id);
            faStyles.textContent = dom.css();
            shadowRoot.appendChild(faStyles);
        }
    },
};
