<template>
  <Header
    headerText="Sunrise adventure"
    subtitleText="Come and enjoy this adventure with us. Fishing in the sunrise is one of the most exciting experiences!"
  />
  <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
  <div class="flexbox-container-reservations">
    <h1>Reservations</h1>
    <!-- <ul>
      <li>New reservation</li>
      <li>Quick reservation</li>
      <li>Generate report</li>
    </ul> -->
  </div>

  <div class="flexbox-container">
    <table class="reservations-table">
      <thead>
        <tr>
          <th>Service</th>
          <th>From</th>
          <th>To</th>
          <th>Client</th>
          <th>Number of guests</th>
          <th>Price</th>
        </tr>
      </thead>
      <tbody v-for="res in reservations" :key="res.id">
        <tr>
          <td class="left">{{ res.service }}</td>
          <td class="left">{{ res.from }}</td>
          <td class="left">{{ res.to }}</td>
          <td class="left">{{ res.client }}</td>
          <td class="left">{{ res.numberOfGuests }}</td>
          <td class="left">{{ res.price }}</td>
        </tr>
      </tbody>
    </table>
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
  computed: {},
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
      reservations: [
        {
          id: 1,
          service: "Sunset adventure",
          from: "01.01.2021 12:00h",
          to: "02.01.2021 12:00h",
          client: "Aco Pejovic",
          numberOfGuests: 10,
          price: 400,
        },
        {
          id: 2,
          service: "Sunrise adventure",
          from: "01.01.2021 12:00h",
          to: "02.01.2021 12:00h",
          client: "Slavko Banjac",
          numberOfGuests: 10,
          price: 500,
        },
        {
          id: 3,
          service: "Sunset fishing",
          from: "01.01.2021 12:00h",
          to: "02.01.2021 12:00h",
          client: "Aca Lukas",
          numberOfGuests: 3,
          price: 350,
        },
        {
          id: 1,
          service: "Crazy barbeque adventure",
          from: "01.01.2021 12:00h",
          to: "02.01.2021 12:00h",
          client: "Zdravko Colic",
          numberOfGuests: 2,
          price: 1500,
        },
      ],
    };
  },
};
</script>

<style scoped>
.flexbox-container {
  padding: 50px;
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
}

.flexbox-container-reservations {
  padding: 50px;
  padding-bottom: 1px;
  display: flex;
  justify-content: space-evenly;
  align-items: flex-start;
  flex-direction: column;
}

.reservations-table {
  border: solid 1px #ddeeee;
  border-collapse: collapse;
  border-spacing: 0;
  font: normal 13px Arial, sans-serif;
}

.reservations-table thead th {
  background-color: #ddefef;
  border: solid 1px #ddeeee;
  color: #336b6b;
  padding: 10px;
  text-align: left;
  text-shadow: 1px 1px 1px #fff;
}

.reservations-table tbody td {
  border: solid 1px #ddeeee;
  color: #333;
  padding: 10px;
  text-shadow: 1px 1px 1px #fff;
}

.left {
  text-align: left;
  width: 250px;
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
  width: 100%;
  box-sizing: border-box;
  border: 0;
  font-size: 16px;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

button:hover {
  background-color: #ccbf05;
}

ul li {
  list-style: none;
  position: relative;
  padding: 10px;
}

li:hover {
  background-color: #c8e6e4;
}

h1 {
  padding: 0px;
}
</style>
