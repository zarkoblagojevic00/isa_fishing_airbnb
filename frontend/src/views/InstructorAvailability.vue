<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <h1>Availability overview</h1>
        <!-- <button @click="$router.push('/instructor/availability/edit')">
            Edit availability
        </button> -->
    </div>
    <!-- use custom calendar with template  -->
    <div class="flexbox-container">
        <InstructorScheduler class="calendar" />
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import InstructorScheduler from "@/components/InstructorScheduler.vue";
import moment from "moment";

export default {
    name: "InstructorServices",
    components: {
        Navbar,
        InstructorScheduler,
    },
    mounted() {},
    data() {
        return {
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
            range: {},
        };
    },
    methods: {
        getDatesWithinRange(start, end) {
            //let startDay = new Date(moment(new Date(start)).startOf("day"));
            let startDay = new Date(start);
            let endDay = new Date(end);
            var datesWithinRange = [];
            for (var d = startDay; d <= endDay; d.setDate(d.getDate() + 1)) {
                datesWithinRange.push(new Date(d));
            }

            let validationDates = datesWithinRange.map((date) => {
                return moment(new Date(date)).startOf("day").format();
            });

            if (
                validationDates.indexOf(
                    moment(new Date(end)).startOf("day").format()
                ) == -1
            ) {
                datesWithinRange.push(endDay);
            }
            return datesWithinRange;
        },
    },
};
</script>

<style scoped>
.flexbox-container {
    display: flex;
    justify-content: space-between;
    padding: 50px;
    padding-bottom: 20px;
    padding-top: 20px;
}
.calendar {
    min-height: 400px;
    min-width: 400px;
}
.booked-slot {
    background: rgb(248, 125, 150);
}
.free-slot {
    background: rgb(20, 220, 110);
}
button {
    background-color: #fff000;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 15px;
    text-align: center;
    transition: 200ms;
    height: 50px;
    box-sizing: border-box;
    border: 0;
    font-size: 16px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
    margin-top: 7px;
}
button:hover {
    background-color: #ccbf05;
}

.calendar {
    height: 120vh;
}
</style>
