<template class="whole">
    <div class="header">
        <div class="backdrop">
            <div class="logo-and-title">
                <h1 class="title">ZAMISR</h1>
            </div>
            <div class="content">
                <div
                    class="header-item"
                    v-for="item in menuitems"
                    :key="item.text"
                >
                    {{ item.text }}
                    <div class="dropdown-menu" v-if="item.options.length > 0">
                        <div
                            class="dropdown-item"
                            v-for="options in item.options"
                            :key="options.text"
                            @click="options.invoke"
                        >
                            {{ options.text }}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "GeneralHeader",
    data() {
        return {};
    },
    props: {
        menuitems: Object,
    },
};
</script>
<!-- Structure of the prop
menuitems: [
    {
        text: 'home page', < The text displayed
        options: [     < The submenu options
            {
                text: 'login', < Text of submenu
                invoke: () => window.location.href = "/Login" < Vue it changes
            }
        ]
    },
],
-->
<style scoped>
.header {
    width: 100%;
    min-height: 300px;
    display: flex;
    position: relative;
    background-image: url("../assets/fishing-header.jpg");
    background-position: top;
    background-size: cover;
}

.backdrop {
    background-color: rgba(0, 0, 0, 0.5);
    width: 100%;
    min-height: 300px;
    display: flex;
    position: relative;
    flex-direction: column;
    align-items: center;
}

.logo-and-title {
    height: calc(100% - 100px);
    display: flex;
    justify-content: center;
    align-items: center;
    font-family: "Brush Script MT", cursive;
    color: rgba(255, 255, 255, 0.8);
    font-size: max(2vw, 36px);
}

.content {
    min-height: 80px;
    width: 60%;
    min-width: 300px;
    position: relative;
    display: flex;
    flex-flow: row wrap;
    justify-content: space-between;
}

.header-item {
    width: auto;
    color: rgba(255, 255, 255, 0.7);
    font-size: 20px;
    margin-left: 10px;
    margin-right: 10px;
    display: flex;
    justify-content: center;
    align-items: center;
    text-transform: uppercase;
    text-decoration: underline;
    background-color: transparent;
    border: none;
    outline: none;
    position: relative;
    cursor: pointer;
}

.dropdown-menu {
    position: absolute;
    top: 60px;
    width: auto;
    min-width: 100%;
    display: none;
    background-color: rgba(0, 0, 0, 0.3);
    z-index: 1;
}

.header-item:hover .dropdown-menu {
    display: block;
    opacity: 1;
}

.dropdown-item {
    padding: 10px;
    border-bottom: 1px solid #c3c3c3;
    font-size: 16px;
    width: auto;
}
</style>
