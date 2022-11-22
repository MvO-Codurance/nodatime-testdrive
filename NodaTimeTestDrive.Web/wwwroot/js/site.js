const renderClocks = () => {
    fetch("https://localhost:7103/worldclocks")
        .then(response => response.json())
        .then(clocks => {
            
            const container = $("#clocks-container");
            container.empty();
            
            for (let i = 0; i < clocks.length; i++) {
                let clock = clocks[i];
                container.append(
                    $("<p/>")
                        .append(
                            $("<strong/>").text(clock.timezone)
                        )
                        .append(
                            $("<span/>").text(" ")
                        )
                        .append(
                            $("<span/>").text(new Date(Date.parse(clock.localDateTime)).toLocaleString())
                        )
                );
            } 
        });
}

$(document).ready(function () {
    renderClocks();
    setInterval(renderClocks, 1000);
});