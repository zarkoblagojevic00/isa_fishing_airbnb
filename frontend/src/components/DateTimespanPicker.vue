<template>
    <Datepicker
        class="date-picker"
        v-model="date"
        placeholder="Select a date"
        :clearable="false"
        :enableTimePicker="false"
        :minDate="minDate"
    />
    <Datepicker
        class="time-picker date-picker"
        time-picker
        v-model="fromTime"
        placeholder="Select start range"
        :clearable="false"
        hideInputIcon
    />
    <Datepicker
        class="time-picker date-picker"
        time-picker
        v-model="toTime"
        placeholder="Select end range"
        :clearable="false"
        hideInputIcon
    />
</template>

<script>
import Datepicker from "vue3-date-time-picker";
import "vue3-date-time-picker/dist/main.css";
import moment from "moment";

export default {
    components: {
        Datepicker,
    },
    props: {
        modelValue: Array,
    },
    data() {
        return {
            fromTime: {
                hours: new Date().getHours(),
                minutes: new Date().getMinutes(),
            },
            toTime: {
                hours: new Date().getHours() + 3,
                minutes: new Date().getMinutes(),
            },
            date: moment().add(1, "days").toDate(),
            minDate: moment().add(1, "days").startOf("day").toDate(),
        };
    },

    mounted() {
        this.emitChanges();
    },

    methods: {
        emitChanges() {
            const fTime = moment(this.date)
                .startOf("day")
                .add(this.fromTime.hours, "hours")
                .add(this.fromTime.minutes, "minutes")
                .toDate();
            const tTime = moment(this.date)
                .startOf("day")
                .add(this.toTime.hours, "hours")
                .add(this.toTime.minutes, "minutes")
                .toDate();
            this.$emit("update:modelValue", [fTime, tTime]);
        },
    },

    watch: {
        fromTime({ hours: newHours, minutes: newMinutes }) {
            if (newHours > this.toTime.hours) {
                this.toTime.hours = newHours;
                this.toTime.minutes = newMinutes;
            } else if (newHours == this.toTime.hours) {
                this.toTime.minutes =
                    this.toTime.minutes > newMinutes
                        ? this.toTimeMinutes
                        : newMinutes;
            }
            this.emitChanges();
        },

        toTime({ hours: newHours, minutes: newMinutes }) {
            if (newHours < this.fromTime.hours) {
                this.fromTime.hours = newHours;
                this.fromTime.minutes = newMinutes;
            } else if (newHours == this.fromTime.hours) {
                this.fromTime.minutes =
                    this.fromTime.minutes < newMinutes
                        ? this.fromTime.minutes
                        : newMinutes;
            }
            this.emitChanges();
        },
        date() {
            this.emitChanges();
        },
    },
};
</script>

<style scoped>
.date-picker {
    width: 100%;
}

.time-picker {
    margin-top: 0.5em;
}
</style>
