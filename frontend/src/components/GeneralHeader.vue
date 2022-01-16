<template class="whole">
    <div class="header">
        <div class="backdrop">
            <div class="logo-and-title">
                <h1 class="title">ZAMISR</h1>
            </div>
            <nav id="navbar">
                <div class="menu">
                    <div
                        class="header-item underline-container"
                        v-for="item in menuitems"
                        :key="item.text"
                    >
                        {{ item.text }}
                        <div
                            class="dropdown-menu"
                            v-if="item.options.length > 0"
                        >
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
            </nav>
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
    min-height: 450px;
    display: flex;
    position: relative;
    background-image: url("../assets/fishing-header.jpg");
    background-position: top;
    background-size: cover;
    background-attachment: fixed;
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

#navbar {
    background-color: rgba(0, 0, 0, 0.4);
    min-height: 60px;
    width: 100%;
    display: flex;
    justify-content: center;
    margin-top: 40px;
}

.menu {
    min-height: 100%;
    min-width: 80%;
    position: relative;
    display: flex;
    justify-content: space-between;
}

.header-item {
    width: 150px;
    color: rgba(255, 255, 255, 0.7);
    font-size: 20px;
    margin-left: 10px;
    margin-right: 10px;
    display: flex;
    justify-content: center;
    align-items: center;
    text-transform: uppercase;
    background-color: transparent;
    border: none;
    outline: none;
    position: relative;
    cursor: pointer;
}

.underline-container {
    background-image: -webkit-linear-gradient(#006b38ff, #006b38ff),
        -webkit-linear-gradient(transparent, transparent);
    background-image: -moz-linear-gradient(#006b38ff, #006b38ff),
        -moz-linear-gradient(transparent, transparent);
    background-image: -ms-linear-gradient(#006b38ff, #006b38ff),
        -ms-linear-gradient(transparent, transparent);
    background-image: -o-linear-gradient(#006b38ff, #006b38ff),
        -o-linear-gradient(transparent, transparent);
    background-image: linear-gradient(#006b38ff, #006b38ff),
        linear-gradient(transparent, transparent);

    background-size: 0 5px, auto;
    background-repeat: no-repeat;
    background-position: center bottom;

    -webkit-transition: all 0.5s ease;
    -moz-transition: all 0.5s ease;
    -ms-transition: all 0.5s ease;
    -o-transition: all 0.5s ease;
    transition: all 0.5s ease;
}

.underline-container:hover {
    background-size: 100% 5px, auto;
}

.dropdown-menu {
    position: absolute;
    top: 60px;
    width: 150px;
    min-width: 100%;
    display: none;
    background-color: rgba(0, 0, 0, 0.6);
    border-radius: 0 0 5px 5px;
    z-index: 1;
}

.header-item:hover .dropdown-menu {
    display: block;
    opacity: 1;
}

.dropdown-item {
    padding: 10px;
    border-bottom: 2px solid #aaa;
    background-color: rgba(0, 0, 0, 0.4);
    font-size: 16px;
    width: auto;
    border-radius: 0 0 3px 3px;
    -webkit-transition: all 0.3s ease-out;
    -moz-transition: all 0.3s ease;
    -ms-transition: all 0.3s ease;
    -o-transition: all 0.3s ease;
    transition: all 0.3s ease;
}

.dropdown-item:hover {
    background-color: rgba(0, 0, 0, 0.8);
}
</style>
