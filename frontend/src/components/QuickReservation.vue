<template>
  <div class="card">
    <p><b> Quick reservation</b></p>
    <div class="container">
      <table>
        <tbody v-for="offer in reservationOffers" :key="offer.id">
          <tr>
            <td class="left">start time:</td>
            <td class="right">{{ dateFormat(offer.date) }}</td>
          </tr>
          <tr>
            <td class="left">duration:</td>
            <td class="right">{{ offer.duration }}</td>
          </tr>
          <tr>
            <td class="left">place:</td>
            <td class="right">{{ offer.place }}</td>
          </tr>
          <tr>
            <td class="left">max people:</td>
            <td class="right">{{ offer.maxNumberOfPeople }}</td>
          </tr>
          <tr>
            <td class="left">price:</td>
            <td class="right">{{ offer.price }}</td>
          </tr>
          <tr class="additional">
            <td class="left">additional services:</td>
            <td class="right">
              <div v-for="service in offer.additionalServices" :key="service">
                {{ service }}
              </div>
            </td>
          </tr>
          <tr>
            <td class="left"></td>
            <td class="right button-book">
              <button>Book</button>
            </td>
          </tr>
          <hr />
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import moment from "moment";

export default {
  data() {
    return {
      reservationOffers: [
        {
          id: 1,
          date: new Date(),
          place: "New York",
          duration: "1h",
          maxNumberOfPeople: 5,
          additionalServices: [
            "instructor has a boat",
            "instructor is funny",
            "instructor is smart",
          ],
          price: 300,
        },
        {
          id: 2,
          date: new Date(),
          place: "London",
          duration: "2h",
          maxNumberOfPeople: 3,
          additionalServices: ["instructor has a boat", "instructor is funny"],
          price: 500,
        },
      ],
    };
  },
  methods: {
    dateFormat(value) {
      return moment(value).format("YYYY-MM-DD HH:mm");
    },
  },
};
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  min-width: 500px;
  max-width: 500px;
  cursor: pointer;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
  padding: 2px 16px;
}

.img {
  width: 100%;
}

.left {
  text-align: left;
  width: 250px;
}

.right {
  text-align: left;
  width: 250px;
}

.additional {
  background-color: lightgray;
}

.button-book {
  display: flex;
  justify-content: center;
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

p {
  font-size: 26px;
}
</style>
