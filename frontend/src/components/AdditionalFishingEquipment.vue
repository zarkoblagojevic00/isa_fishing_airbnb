<template>
  <p><b> Additional fishing equipment </b></p>
  <div class="container">
    <table class="equipment-table">
      <thead>
        <tr>
          <th>Equipment</th>
        </tr>
      </thead>
      <tbody v-for="eq in fishingEquipment" :key="eq.id">
        <tr>
          <td class="left">{{ eq.name }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  data() {
    return {
      fishingEquipment: [
        {
          id: 1,
          name: "Fishing stick",
        },
        {
          id: 2,
          name: "Fishing chair",
        },
      ],
    };
  },
};
</script>

<style scoped>
.container {
  padding: 2px 16px;
}

.left {
  text-align: left;
  width: 250px;
}

.equipment-table {
  border: solid 1px #ddeeee;
  border-collapse: collapse;
  border-spacing: 0;
  font: normal 13px Arial, sans-serif;
}
.equipment-table thead th {
  background-color: #ddefef;
  border: solid 1px #ddeeee;
  color: #336b6b;
  padding: 10px;
  text-align: left;
  text-shadow: 1px 1px 1px #fff;
}
.equipment-table tbody td {
  border: solid 1px #ddeeee;
  color: #333;
  padding: 10px;
  text-shadow: 1px 1px 1px #fff;
}
</style>
