<template>
  <div class="card">
    <div class="container">
      <table class="allowed-table">
        <thead>
          <tr>
            <th>Allowed</th>
          </tr>
        </thead>
        <tbody v-for="rule in rulesAllowed" :key="rule.id">
          <tr>
            <td class="left">{{ rule.name }}</td>
          </tr>
        </tbody>
      </table>

      <table class="forbidden-table">
        <thead>
          <tr>
            <th>Forbidden</th>
          </tr>
        </thead>
        <tbody v-for="rule in rulesForbidden" :key="rule.id">
          <tr>
            <td class="left">{{ rule.name }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <div>
      <div>
        <p v-if="freeCancellation" class="left">Free cancellation!</p>
        <p v-else class="left">
          Upon cancellation, instructor keeps 10% of the money paid.
        </p>
      </div>
      <div>
        <div class="left">Max people: {{ maxPeople }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      rulesAllowed: [
        {
          id: 1,
          name: "Alcohol",
        },
        {
          id: 2,
          name: "Food",
        },
        {
          id: 3,
          name: "Soft beverages",
        },
      ],
      rulesForbidden: [
        {
          id: 1,
          name: "Smoking",
        },
        {
          id: 2,
          name: "Fighting",
        },
        {
          id: 3,
          name: "Programming",
        },
      ],
      freeCancellation: true,
      maxPeople: 10,
    };
  },
};
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  cursor: pointer;
  padding: 20px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
  display: flex;
  justify-content: space-between;
  padding: 2px 16px;
}

.left {
  text-align: left;
  width: 250px;
  margin-left: 20px;
}

.right {
  text-align: left;
  width: 250px;
}

.allowed-table {
  border: solid 1px #ddeeee;
  border-collapse: collapse;
  border-spacing: 0;
  font: normal 13px Arial, sans-serif;
}
.allowed-table thead th {
  background-color: #ddefef;
  border: solid 1px #ddeeee;
  color: #336b6b;
  padding: 10px;
  text-align: left;
  text-shadow: 1px 1px 1px #fff;
}
.allowed-table tbody td {
  border: solid 1px #ddeeee;
  color: #333;
  padding: 10px;
  text-shadow: 1px 1px 1px #fff;
}

.forbidden-table {
  border: solid 1px #ddeeee;
  border-collapse: collapse;
  border-spacing: 0;
  font: normal 13px Arial, sans-serif;
}
.forbidden-table thead th {
  background-color: #f09f9f;
  border: solid 1px #ddeeee;
  color: #6b3333;
  padding: 10px;
  text-align: left;
  text-shadow: 1px 1px 1px #fff;
}
.forbidden-table tbody td {
  border: solid 1px #ddeeee;
  color: #333;
  padding: 10px;
  text-shadow: 1px 1px 1px #fff;
}
</style>
