export default {
    data() {
        return {
            equipment: null,
        };
    },

    computed: {
        additionalEquipment() {
            return Object.entries(this.equipment).reduce(
                (acc, [key, item]) =>
                    item.isAdditional ? { ...acc, [key]: item } : acc,
                {}
            );
        },

        additionalEquipmentEntries() {
            return Object.entries(this.additionalEquipment);
        },

        baseEquipment() {
            return Object.entries(this.equipment).reduce(
                (acc, [key, item]) =>
                    !item.isAdditional ? { ...acc, [key]: item } : acc,
                {}
            );
        },

        baseEquipmentEntries() {
            return Object.entries(this.baseEquipment);
        },

        notChosenEquipment() {
            return Object.entries(this.equipment).reduce(
                (acc, [key, item]) =>
                    !item.isAdditional || item.isChosen
                        ? acc
                        : { ...acc, [key]: item },
                {}
            );
        },

        chosenEquipment() {
            return Object.entries(this.equipment).reduce(
                (acc, [key, item]) =>
                    !item.isAdditional || item.isChosen
                        ? { ...acc, [key]: item }
                        : acc,
                {}
            );
        },

        finalCostPerDay() {
            return Object.entries(this.equipment).reduce(
                (acc, [, item]) =>
                    !item.isAdditional || item.isChosen
                        ? acc + item.price
                        : acc,
                0
            );
        },
    },

    methods: {
        parseEquipment(equipString) {
            this.equipment = equipString.split(";").reduce((acc, curr) => {
                const [item, priceStr] = curr.split(":");
                const price = parseFloat(priceStr);
                acc[item] = {
                    price,
                    isAdditional: price !== 0,
                    isChosen: false,
                };
                return acc;
            }, {});
        },
        chosenEquipmentToString() {
            return Object.entries(this.equipment)
                .reduce((acc, [key, item]) => {
                    const next =
                        !item.price || item.isChosen
                            ? `;${key}:${item.price}`
                            : "";
                    return `${acc}${next}`;
                }, "")
                .substring(1);
        },
        addEquipment(equipmentKey) {
            this.equipment[equipmentKey].isChosen = true;
        },
        removeEquipment(equipmentKey) {
            this.equipment[equipmentKey].isChosen = false;
        },
    },
};
