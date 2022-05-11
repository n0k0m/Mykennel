using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mykennel.Utility
{
    /*
    //                         SZÉLSŐSÉGES ADATOK TESZTELÉSÉRE!                                //
    //----------------------------------------------------------------------------------------//
    public static class TestKennels
    {
        // KennelId - adatbázisból

        public static string[] KennelName =
            {
            "c",
            "sa",
            "airfreshener",
            "spoon clamper",
            "cat",
            "newspaper",
            "computer newspaper newspaper",
            "candy wrapper",
            "spoon",
            "CD spoonspoon",
            "rug drillpress",
            "bag drillpress drillpress spoonspoon spoon",
            "m",
            "tv",
            "thermostat nail clippers",
            "thread mouse padmouse padmouse pad",
            "fo",
            "money",
            "packingpeanuts braceletbracelet bracelet",
            "wallet nail clippers nail clippers nail",
            "bracelet",
            "nail clippers",
            "h",
            "claypot nail clippers drillpressdrillpress",
            "keyboard",
            "drillpress",
            "bottlecap",
            "outlet",
            "cup",
            "chalk"
        };
        public static string[] URLName =
            {
            "condition",
            "sandal",
            "airfresher",
            "clamp",
            "cat",
            "newspaper",
            "computer",
            "canwrapper",
            "spoon",
            "cd",
            "rug",
            "bag",
            "mousepad",
            "tv",
            "thermostat",
            "thread",
            "fork",
            "money",
            "packingpea",
            "wallet",
            "bracelet",
            "nailclipp",
            "hanger",
            "claypot",
            "keyboard",
            "drillpress",
            "bottlecap",
            "outlet",
            "cup",
            "chalk"
        };
        public static string[] City =
            {
            "H",
            "Brownsburg",
            "Palm City Marquette City Town",
            "Henderson Marquette Brownsburg Palm City Clay"
        };

        public static string[] PostalCode =
            {
            "7",
            "H311",
            "231321",
            "H12364354"
        };

        public static string[] Address =
            {
            "A",
            "Avenue Gree 2",
            "Shadow Brook Avenue Meadville Street 1",
            "North Shadow Brook Avenue Meadville Street Home Very 23",
            "8975 North Shadow Brook Avenue Home Very Brook Avenue Meadville Street Home Very"
        };

        public static string[] ShortDescription =
            {
            null,
            "L",
            "Lorem ipsum dolor sit amet, consectetuer adipiscin",
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m",
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium.",
            "Lorem ipsum dolor sit amet, commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu"
        };

        public static string[] Description =
            {
            null,
            "L",
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu",
            "Lorem ipsum dolor sit amet, Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. N",
            "Lorem Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, met",
            "Lorem ipsum dolor sit amet,  commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestib"

        };

        public static string[] Logo =
            { null,
                @"\images\testImages\logo_landscape.png",
                @"\images\testImages\logo_landscape_extreme.png",
                @"\images\testImages\logo_portrait.png",
                @"\images\testImages\logo_portrait_extreme.png",
                @"\images\testImages\logo_rectangle.png",
        };

        public static string[] ContactPerson =
        {
            null,
            "S",
            "Raegan Sellers",
            "Cason Harper Maddison Arias",
            "Desiree Huang Jerimiah Chapman Kendal Baxter",
        };

        // CountryId adatbázisból
        // ApplicationUserId adatbázisból

        //                         VALÓSZERŰ ADATOKKAL VALÓ FELTÖLTÉSRE!                            //
        //----------------------------------------------------------------------------------------//

    }
    */
        public static class TestKennels
        {
            // KennelId - adatbázisból

            public static string[] KennelName =
                {
            "Cute Munchers",
            "Breeders Digest Kennel",
            "Happiness Puppy Kennel",
            "Smart Puppies",
            "The Most Wonderful Puppies Doggery",
            "Yorkie's Fun",
            "Hello Dog",
            "The Tale of Doggies",
            "Caesar kennel",
            "Once Upon a Time kennel",
            "King kennel",
            "Hobby kennel",
            "Ultimate dogs",
            "The Furriest",
            "Paws",
            "Cute Pets",
            "Grande Doggery",
            "Cool Kennel",
            "Working Paws",
            "Simple 9 Kennel",
            "Working dogs",
            "Hello Pet",
            "Dogzone",
            "We Love Dogs",
            "Dog Caffe",
            "Breedfully kennel"
        };
            public static string[] URLName =
                {
            "cutemunchers",
            "breedersdigest",
            "happinesspuppy",
            "smartpuppy",
            "mostwonderful",
            "yorkiesfun",
            "hellodog",
            "taleofdoggies",
            "caesar",
            "onceuponatime",
            "kingkennel",
            "hobbykennel",
            "ultimatedog",
            "furriest",
            "paws",
            "cutepets",
            "grandedoggery",
            "coolkennel",
            "workingpaws",
            "simple9",
            "workingdogs",
            "hellopet",
            "dogzone",
            "welovedogs",
            "dogcaffee",
            "breedfully"
        };
            public static string[] City =
                {
            "Fortham",
            "Windol",
            "Backfolk",
            "Summerside",
            "Costsstead",
            "Farmingmouth",
            "Clammouth",
            "Hapley",
            "Aelside",
            "West Hamdale",
            "Beachfield",
            "Elborough",
            "Capness",
            "Hapdol Falls",
            "Cruxby",
            "Cruxdol",
            "Wingburgh",
            "Freeport",
            "Hallville",
            "Beachness Falls"
        };

            public static string[] PostalCode =
                {
            "H-6800",
            "10544",
            "HU1210",
            "PL9008",
            "7700",
            "9700",
            "H-3200",
            "PL22001",
            "8800",
            "110020",
            "A-8000",
            "H-2010",
            "A-7610",
        };

            public static string[] Address =
                {
            "South Sleepy Hollow Lane 10.",
            "South Wentworth Ave. 200.",
            "396 Primrose Lane",
            "Peg Shop Ave. 6.",
            "Mill Pond Street 7748",
            "5 East Cherry Street",
            "159 George Dr.",
            "960 Valley Farms St.",
            "Somerset St. 99 ",
            "Valley View Lane 149 ",
            "Santa Clara Lane 9400",
            "179 High Noon Dr.",
            "Euclid Ave. 737 ",
            "76 North Adams Street",
            "Birchwood Street 20.",
            "239 High Ridge Street",
            "816 Old Durham Lane"
        };

            public static string[] ShortDescription =
                {
            null,
            "People have been breeding dogs since prehistoric times. The earliest dog breeders used wolves to create domestic dogs.",
            "A dog or a cat loving us unconditionally, every day, very faithfully.” – Jon Katz",
            "We are crazy about dogs, so crazy that we created thousands of content-rich pages.",
            "Our commitment to dogs and their owners has made us a world guide in dog breeds.",
            "Dog breeds have been developed in many countries and on every continent except Antarctica. ",
            "There are pros and cons to everything out there, and pets are no exception.",
            "Every year the American Kennel Club releases our ranking of the most popular dog breeds.",
            "Was born of a love for purebred dogs.",
            "There are some drastic differences between the top dog breeds",
            "Four-legged pals that captured the hearts of US citizens.",
            "Top dog breeds from past years’ lists remain.",
            "Your dog’s personality may have little to do with its breed, a new US study reveals.",
            "Originally bred to “bait” bulls, the breed evolved into all-around farm dogs, and later moved into the house.",
        };

            public static string[] Description =
                {
            null,
            "Research confirms what dog lovers know – every pup is truly an individual. Many of the popular stereotypes about the behaviour of golden retrievers, poodles or schnauzers, for example, aren’t supported by science, according to a new US study. “There is a huge amount of behavioural variation in every breed, and at the end of the day, every dog really is an individual,” said study co-author and University of Massachusetts geneticist Elinor Karlsson.",
            "That kind of enthusiasm from pet owners inspired Karlsson’s latest scientific inquiry. She wanted to know to what extent are behavioural patterns inherited – and how much are dog breeds associated with distinctive and predictable behaviours? The answer: while physical traits such as a greyhound’s long legs or a Dalmatian’s spots are clearly inherited, breed is not a strong predictor of any individual dog’s personality.",
            "Dogs, like humans, are highly social animals and this similarity in their overall behavioural pattern accounts for their trainability, playfulnes and ability to fit into human households and social situations. This similarity has earned dogs a unique position in the realm of interspecies relationships. The loyalty and devotion that dogs demonstrate as part of their natural instincts as pack animals closely mimics the human idea of love and friendship, leading many dog owners to view their pets as full-fledged family members. The common name for the domestic dog is ‘Canis familiaris‘, a species of the dog family ‘Canidae‘. The dog is generally considered the ‘first’ domesticated animal.",
            "DOGS – COMPANIONS ACROSS THE AGES. It is estimated that for more than 12,000 years the dog has lived with humans as a hunting companion, protector and friend. A dog is one of the most popular pets in the world and has been referred to as ‘mans best friend’. Whether you are poor or rich, a dog will be faithful and loyal to you and love you to bits. A pet dog will fit easily into family life and environment, they of course need caring for as any other pet – feeding, grooming, bathing and when ill, will need a visit to the vets. Dogs thrive on affection and will happily wag its tail when showered with love and attention. Dogs will also sit and sulk if they get told off for doing something wrong.",
            "A well-mannered dog must be trained properly and with patience and perseverance. You must never mistreat a dog at any time as not only is it very wrong to mistreat any animal, but some will bite in their own defense. Larger breeds need considerably more exercise over a larger area than medium sized or small dogs. You can take them out into the countryside or to your local park or recreation ground. Here, they can run about and play games and get the exercise they need to keep them fit and healthy. Some breeds have been selectively bred for excellence in detecting scents.",
            "What information a dog actually detects when he is scenting is not perfectly understood. Although once a matter of debate, it now seems to be well established that dogs can distinguish two different types of scents when trailing an air scent from some person or thing that has recently passed by, as well as a ground scent that remains detectable for a much longer period.",
            "The term “dog days” has nothing to do with dogs. It dates back to Roman times, when it was believed that Sirius, the Dog Star, added its heat to that of the sun from July 3 to August 11, creating exceptionally high temperatures.",
            "Dogs may not have as many taste buds as we do (they have about 1,700 on their tongues, while we humans have about 9,000), but that doesn’t mean they’re not discriminating eaters. They have over 200 million scent receptors in their noses (we have only 5 million) so it’s important that their food smells good and tastes good.",
            "A dog’s whiskers — found on the muzzle, above the eyes and below the jaws — are technically known as vibrissae. They are touch-sensitive hairs than actually sense minute changes in airflow.",
        };

            public static string[] Logo =
                { null,
                @"\images\testImages\logos\1.png",
                @"\images\testImages\logos\2.png",
                @"\images\testImages\logos\3.png",
                @"\images\testImages\logos\4.png",
                @"\images\testImages\logos\5.png",
                @"\images\testImages\logos\6.png",
                @"\images\testImages\logos\7.png",
                @"\images\testImages\logos\8.png",
                @"\images\testImages\logos\9.png"
        };

            public static string[] ContactPerson =
            {
            null,
            "Aneesha Hutchinson",
            "Rosalie Davidson",
            "Lisa-Marie Douglas",
            "Amber-Rose May",
            "Julia Glover",
            "Eesha Macgregor",
            "William Steadman",
            "Kacy Decker",
            "Zakaria Love",
            "Dawud Coombes",
            "Szymon Rodrigues",
            "Aden Dunlop",
            "Lily-Mae Harris",
            "Chenai Phelps",
            "Jolene Bloggs",
            "Ioana Norris",
            "Zak Olson",
            "Jozef Mcneil",
            "Ralphie Field",
            "Asia Benson"
        };

            // CountryId adatbázisból
            // ApplicationUserId adatbázisból

        }
    }