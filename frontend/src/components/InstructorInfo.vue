<template>
    <div class="card">
        <img src="../assets/avatar.png" alt="Avatar" style="width: 100%" />
        <div class="container">
            <h4><b>Instructor John Doe</b></h4>
            <p>Adrenaline addict, ready to give you a good time.</p>
        </div>
    </div>
</template>

<script>
export default {
    name: "InstructorInfo",
};
</script>

<style scoped>
.card {
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    max-width: 500px;
}

.card:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
    padding: 2px 16px;
}
</style>
