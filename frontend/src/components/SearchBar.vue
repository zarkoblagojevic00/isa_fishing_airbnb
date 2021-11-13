<template>
  <div class="search-container">
    <form>
      <input type="text" :placeholder="searchPlaceholder" />
      <button type="submit">
        <font-awesome-icon icon="search"></font-awesome-icon>
      </button>
    </form>
  </div>
</template>

<script>
export default {
  props: {
    searchPlaceholder: String,
  },
  methods: {},
};
</script>

<style scoped>
.search-container {
  display: flex;
  justify-content: flex-end;
}
input[type="text"] {
  padding: 6px;
  margin-top: 8px;
  font-size: 17px;
  width: 300px;
}

.search-container button {
  padding: 8px;
  margin-top: 8px;
  background: #ddd;
  font-size: 17px;
  border: none;
  cursor: pointer;
}

.search-container button:hover {
  background: #ccc;
}
</style>
