function map(dataURL) {
    // canvas resolution
    var width = 1000,
        height = 600;

    // projection-settings for mercator    
    var projection = d3.geo.mercator()
        // where to center the map in degrees
        .center([22, 61])
        // zoomlevel
        .scale(500)
        // map-rotation
        .rotate([0, 0]);

    // defines "svg" as data type and "make canvas" command
    var svg = d3.select("div#chart").html("").append("svg")
        .attr("width", width)
        .attr("height", height);

    // defines "path" as return of geographic features
    var path = d3.geo.path()
        .projection(projection);

    // group the svg layers 
    var g = svg.append("g");
    var data = undefined;
    var max = undefined;
    var min = undefined;
    var avg = undefined;
    d3.json(dataURL, function (error, data) {
        if (error) throw error;
        //  load data and display the map on the canvas with country geometries
        d3.json("https://gist.githubusercontent.com/d3noob/5193723/raw/world-110m2.json", function (error, topology) {
            this.data = data;
            this.max = _.max(data, function (x) { return x.Value; }).Value;
            this.min = _.min(data, function (x) { return x.Value; }).Value;
            this.avg = d3.sum(data, function (x) { return x.Value; }) / data.length;
            that = this;
            g.selectAll("path")
              .data(topojson.object(topology, topology.objects.countries)
                  .geometries)
            .enter()
                .append("svg:a")
               .attr("xlink:href", function (d) {
                   return "http://localhost:4282/Main/Ranking/" + d.id
               })
              .append("svg:path")
              .attr("d", path)
              .attr("class", "country")
              .attr("id", function (d) { return d.id })
                    .style("fill", function (d) {
                        return fill(that, d);
                    });

            var legend = svg.selectAll(".legend")
          .data(_.map(data, function (x) { x.Value }))
          .enter()
          .append("g")
              .attr({
                  'class': 'legend',
                  'transform': function (d, i) {
                      return "translate(" + (i * 40) + "," + (height - 40) + ")";
                  }
              });

            legend
                .data([this.min, this.avg, this.max])
                .append("rect")
            .attr({
                'width': 40,
                'height': 20,
                'fill': function (d) {
                    var color = 'grey';
                    if (d > that.avg)
                        color = d3.rgb(0, 51, 0).darker((d / that.max));
                    else
                        color = d3.rgb(255, 0, 0).darker((that.min / d));
                    return color;
                }
                // Problem likely to be arising from here
            });
            legend.data([this.min, this.avg, this.max]).append("text")
            .attr("x", 20)
            .attr("y", 2)
            .attr("dy", ".75em")
            .attr("text-anchor", "middle")
            .attr("fill", "white")
            .text(function (d) {
                if (d > that.avg)
                    return "max";
                if (d < that.avg)
                    return "min";
                return "avg";
            }
                );
        });

    });


    function fill(that, d) {
        var cdata = _.find(that.data, function (x) { return x.CountryCode == d.id; });
        var color = undefined;
        if (cdata == undefined)
            return "grey";
        if (cdata.Value > that.avg)
            color = d3.rgb(0, 51, 0).darker((cdata.Value / that.max));
        else
            color = d3.rgb(255, 0, 0).darker((that.min / cdata.Value));
        return color;
    }
    function type(d) {
        d.CountryCode = +d.CountryCode;
        d.Value = +d.Value;
        return d;
    }
}
var dataUrl = 'getgdpdata';
map(dataUrl);

_.each($('li'),function(li){
    $(li).on("click", function () {
        dataUrl = $(li).attr('data'); 
        map(dataUrl)
  })
})
