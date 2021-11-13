<template>
  <div class="container card">
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
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  transition: 0.3s;
  cursor: pointer;
  padding: 20px;
}

.card:hover {
  box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
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
