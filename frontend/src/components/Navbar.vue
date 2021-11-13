<template>
  <ul>
    <li v-for="(item, index) in navbarItems" :key="index">
      <router-link class="link" :to="formatUrl(item)">
        {{ item }}
      </router-link>
    </li>
  </ul>
</template>

<script>
export default {
  props: {
    navbarItems: Array,
    baseUrl: String,
  },
  methods: {
    formatUrl(input) {
      return this.baseUrl + input.replace(/\s+/g, "-").toLowerCase();
    },
  },
};
</script>

<style scoped>
ul {
  display: flex;
  justify-content: space-evenly;
  list-style-type: none;
  margin: 0;
  padding: 0;
  overflow: hidden;
  background-color: rgb(0, 0, 0);
}

li .link {
  display: block;
  color: white;
  text-align: center;
  padding: 14px 16px;
  text-decoration: none;
}

/* Change the link color to #111 (black) on hover */
li .link:hover {
  background-color: #111;
}
</style>
