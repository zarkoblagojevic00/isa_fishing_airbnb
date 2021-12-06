<template>
    <Header
        headerText="Sunrise adventure"
        subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
    />
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <h1>Availability overview</h1>
        <button>Edit availability</button>
    </div>
    <!-- use custom calendar with template -->
    <div class="flexbox-container">
        <v-calendar
            class="custom-calendar max-w-full"
            :masks="masks"
            :attributes="attributes"
            disable-page-swipe
            is-expanded
        >
            <template v-slot:day-content="{ day, attributes }">
                <div class="slot">
                    <span>
                        <b>{{ day.day }}</b>
                    </span>
                    <div>
                        <div
                            v-for="attr in attributes"
                            :key="attr.key"
                            :class="attr.customData.class"
                        >
                            {{ attr.customData.title }}
                        </div>
                    </div>
                </div>
            </template>
        </v-calendar>
    </div>
</template>

<script>
import Header from "@/components/Header.vue";
import Navbar from "@/components/Navbar.vue";

export default {
    name: "InstructorServices",
    components: {
        Header,
        Navbar,
    },
    data() {
        const month = new Date().getMonth();
        const year = new Date().getFullYear();
        return {
            masks: {
                weekdays: "WWW",
            },
            attributes: [
                {
                    key: 1,
                    customData: {
                        title: "12:00 - 17:45",
                        status: "booked",
                        class: "booked-slot",
                    },
                    dates: new Date(year, month, 1),
                },
                {
                    key: 2,
                    customData: {
                        title: "11:00 - 19:00",
                        status: "booked",
                        class: "booked-slot",
                    },
                    dates: new Date(year, month, 2),
                },
                {
                    key: 3,
                    customData: {
                        title: "14:00 - 17:45",
                        status: "free",
                        class: "free-slot",
                    },
                    dates: new Date(year, month, 5),
                },
                {
                    key: 4,
                    customData: {
                        title: "15:00 - 22:30",
                        status: "free",
                        class: "free-slot",
                    },
                    dates: new Date(year, month, 9),
                },
                {
                    key: 5,
                    customData: {
                        title: "08:00 - 14:30",
                        status: "free",
                        class: "free-slot",
                    },
                    dates: new Date(year, month, 11),
                },
            ],
            services: [
                {
                    id: 1,
                    name: "Sunrise adventure",
                    description: "Promo description",
                    address: "Bulevard of broken dreams 0",
                },
                {
                    id: 2,
                    name: "Sunset adventure",
                    description: "Promo description",
                    address: "Bulevard of broken dreams 1",
                },
                {
                    id: 3,
                    name: "Sunset adventure",
                    description: "Promo description",
                    address: "Bulevard of broken dreams 2",
                },
            ],
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
        };
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
</style>
